﻿namespace Fooidity.Management.AzureIntegration.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Entities;
    using Exceptions;
    using Fooidity.AzureIntegration;
    using Management.Queries;
    using Microsoft.WindowsAzure.Storage.Table;
    using Models;


    public class GetOrganizationQueryHandler :
        IQueryHandler<GetOrganization, Organization>
    {
        readonly AzureManagementSettings _settings;
        readonly ICloudTableProvider _tableProvider;

        public GetOrganizationQueryHandler(ICloudTableProvider tableProvider, AzureManagementSettings settings)
        {
            _tableProvider = tableProvider;
            _settings = settings;
        }

        public async Task<Organization> Execute(GetOrganization query, CancellationToken cancellationToken = new CancellationToken())
        {
            if (query == null)
                throw new ArgumentNullException("query");
            if (string.IsNullOrWhiteSpace(query.UserId))
                throw new ArgumentException("UserId is required");

            CloudTable organizationUserTable = _tableProvider.GetTable(_settings.UserOrganizationIndexTableName);

            TableQuery<UserOrganizationIndexEntity> organizationUserQuery = new TableQuery<UserOrganizationIndexEntity>()
                .Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, query.UserId),
                    TableOperators.And, TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, query.OrganizationId))).Take(1);

            IEnumerable<UserOrganizationIndexEntity> organizationUsers =
                await organizationUserTable.ExecuteQueryAsync(organizationUserQuery, cancellationToken);
            UserOrganizationIndexEntity organizationUser = organizationUsers.SingleOrDefault();
            if (organizationUser == null)
                throw new OrganizationNotFoundException(query.UserId, query.OrganizationId);

            CloudTable organizationTable = _tableProvider.GetTable(_settings.OrganizationTableName);

            TableQuery<OrganizationEntity> organizationQuery = new TableQuery<OrganizationEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal,
                    query.OrganizationId)).Take(1);

            IEnumerable<OrganizationEntity> organizations = await organizationTable.ExecuteQueryAsync(organizationQuery, cancellationToken);
            OrganizationEntity organization = organizations.SingleOrDefault();
            if (organization == null)
                throw new OrganizationNotFoundException(query.UserId, query.OrganizationId);

            return organization;
        }
    }
}