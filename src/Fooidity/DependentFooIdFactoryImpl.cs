﻿namespace Fooidity
{
    public class DependentFooIdFactoryImpl<T> :
        DependentFooIdFactory<T>
        where T : struct, FooId
    {
        public FooId<T, T1> Upon<T1>(FooId<T1> fooId1)
            where T1 : struct, FooId
        {
            return new DependentFooId<T, T1>(fooId1);
        }

        public FooId<T, T1, T2> Upon<T1, T2>(FooId<T1> fooId1, FooId<T2> fooId2)
            where T1 : struct, FooId
            where T2 : struct, FooId
        {
            return new DependentFooId<T, T1, T2>(fooId1, fooId2);
        }

        public FooId<T, T1, T2, T3> Upon<T1, T2, T3>(FooId<T1> fooId1, FooId<T2> fooId2, FooId<T3> fooId3)
            where T1 : struct, FooId
            where T2 : struct, FooId
            where T3 : struct, FooId
        {
            return new DependentFooId<T, T1, T2, T3>(fooId1, fooId2, fooId3);
        }

        public FooId<T, T1, T2, T3, T4> Upon<T1, T2, T3, T4>(FooId<T1> fooId1, FooId<T2> fooId2, FooId<T3> fooId3, FooId<T4> fooId4)
            where T1 : struct, FooId
            where T2 : struct, FooId
            where T3 : struct, FooId
            where T4 : struct, FooId
        {
            return new DependentFooId<T, T1, T2, T3, T4>(fooId1, fooId2, fooId3, fooId4);
        }

        public FooId<T, T1, T2, T3, T4, T5> Upon<T1, T2, T3, T4, T5>(FooId<T1> fooId1, FooId<T2> fooId2, FooId<T3> fooId3, FooId<T4> fooId4, FooId<T5> fooId5)
            where T1 : struct, FooId
            where T2 : struct, FooId
            where T3 : struct, FooId
            where T4 : struct, FooId
            where T5 : struct, FooId
        {
            return new DependentFooId<T, T1, T2, T3, T4, T5>(fooId1, fooId2, fooId3, fooId4, fooId5);
        }

        public FooId<T, T1, T2, T3, T4, T5, T6> Upon<T1, T2, T3, T4, T5, T6>(FooId<T1> fooId1, FooId<T2> fooId2, FooId<T3> fooId3, FooId<T4> fooId4, FooId<T5> fooId5, FooId<T6> fooId6)
            where T1 : struct, FooId
            where T2 : struct, FooId
            where T3 : struct, FooId
            where T4 : struct, FooId
            where T5 : struct, FooId
            where T6 : struct, FooId
        {
            return new DependentFooId<T, T1, T2, T3, T4, T5, T6>(fooId1, fooId2, fooId3, fooId4, fooId5, fooId6);
        }

        public FooId<T, T1, T2, T3, T4, T5, T6, T7> Upon<T1, T2, T3, T4, T5, T6, T7>(FooId<T1> fooId1, FooId<T2> fooId2, FooId<T3> fooId3, FooId<T4> fooId4, FooId<T5> fooId5, FooId<T6> fooId6, FooId<T7> fooId7)
            where T1 : struct, FooId
            where T2 : struct, FooId
            where T3 : struct, FooId
            where T4 : struct, FooId
            where T5 : struct, FooId
            where T6 : struct, FooId
            where T7 : struct, FooId
        {
            return new DependentFooId<T, T1, T2, T3, T4, T5, T6, T7>(fooId1, fooId2, fooId3, fooId4, fooId5, fooId6, fooId7);
        }

        public FooId<T, T1, T2, T3, T4, T5, T6, T7, T8> Upon<T1, T2, T3, T4, T5, T6, T7, T8>(FooId<T1> fooId1, FooId<T2> fooId2, FooId<T3> fooId3, FooId<T4> fooId4, FooId<T5> fooId5, FooId<T6> fooId6, FooId<T7> fooId7, FooId<T8> fooId8)
            where T1 : struct, FooId
            where T2 : struct, FooId
            where T3 : struct, FooId
            where T4 : struct, FooId
            where T5 : struct, FooId
            where T6 : struct, FooId
            where T7 : struct, FooId
            where T8 : struct, FooId
        {
            return new DependentFooId<T, T1, T2, T3, T4, T5, T6, T7, T8>(fooId1, fooId2, fooId3, fooId4, fooId5, fooId6, fooId7, fooId8);
        }

        public FooId<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Upon<T1, T2, T3, T4, T5, T6, T7, T8, T9>(FooId<T1> fooId1, FooId<T2> fooId2, FooId<T3> fooId3, FooId<T4> fooId4, FooId<T5> fooId5, FooId<T6> fooId6, FooId<T7> fooId7, FooId<T8> fooId8, FooId<T9> fooId9)
            where T1 : struct, FooId
            where T2 : struct, FooId
            where T3 : struct, FooId
            where T4 : struct, FooId
            where T5 : struct, FooId
            where T6 : struct, FooId
            where T7 : struct, FooId
            where T8 : struct, FooId
            where T9 : struct, FooId
        {
            return new DependentFooId<T, T1, T2, T3, T4, T5, T6, T7, T8, T9>(fooId1, fooId2, fooId3, fooId4, fooId5, fooId6, fooId7, fooId8, fooId9);
        }

        public FooId<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Upon<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(FooId<T1> fooId1, FooId<T2> fooId2, FooId<T3> fooId3, FooId<T4> fooId4, FooId<T5> fooId5, FooId<T6> fooId6, FooId<T7> fooId7, FooId<T8> fooId8, FooId<T9> fooId9, FooId<T10> fooId10)
            where T1 : struct, FooId
            where T2 : struct, FooId
            where T3 : struct, FooId
            where T4 : struct, FooId
            where T5 : struct, FooId
            where T6 : struct, FooId
            where T7 : struct, FooId
            where T8 : struct, FooId
            where T9 : struct, FooId
            where T10 : struct, FooId
        {
            return new DependentFooId<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(fooId1, fooId2, fooId3, fooId4, fooId5, fooId6, fooId7, fooId8, fooId9, fooId10);
        }

        public FooId<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Upon<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(FooId<T1> fooId1, FooId<T2> fooId2, FooId<T3> fooId3, FooId<T4> fooId4, FooId<T5> fooId5, FooId<T6> fooId6, FooId<T7> fooId7, FooId<T8> fooId8, FooId<T9> fooId9, FooId<T10> fooId10, FooId<T11> fooId11)
            where T1 : struct, FooId
            where T2 : struct, FooId
            where T3 : struct, FooId
            where T4 : struct, FooId
            where T5 : struct, FooId
            where T6 : struct, FooId
            where T7 : struct, FooId
            where T8 : struct, FooId
            where T9 : struct, FooId
            where T10 : struct, FooId
            where T11 : struct, FooId
        {
            return new DependentFooId<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(fooId1, fooId2, fooId3, fooId4, fooId5, fooId6, fooId7, fooId8, fooId9, fooId10, fooId11);
        }

        public FooId<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Upon<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(FooId<T1> fooId1, FooId<T2> fooId2, FooId<T3> fooId3, FooId<T4> fooId4, FooId<T5> fooId5, FooId<T6> fooId6, FooId<T7> fooId7, FooId<T8> fooId8, FooId<T9> fooId9, FooId<T10> fooId10, FooId<T11> fooId11, FooId<T12> fooId12)
            where T1 : struct, FooId
            where T2 : struct, FooId
            where T3 : struct, FooId
            where T4 : struct, FooId
            where T5 : struct, FooId
            where T6 : struct, FooId
            where T7 : struct, FooId
            where T8 : struct, FooId
            where T9 : struct, FooId
            where T10 : struct, FooId
            where T11 : struct, FooId
            where T12 : struct, FooId
        {
            return new DependentFooId<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(fooId1, fooId2, fooId3, fooId4, fooId5, fooId6, fooId7, fooId8, fooId9, fooId10, fooId11, fooId12);
        }

        public FooId<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Upon<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(FooId<T1> fooId1, FooId<T2> fooId2, FooId<T3> fooId3, FooId<T4> fooId4, FooId<T5> fooId5, FooId<T6> fooId6, FooId<T7> fooId7, FooId<T8> fooId8, FooId<T9> fooId9, FooId<T10> fooId10, FooId<T11> fooId11, FooId<T12> fooId12, FooId<T13> fooId13)
            where T1 : struct, FooId
            where T2 : struct, FooId
            where T3 : struct, FooId
            where T4 : struct, FooId
            where T5 : struct, FooId
            where T6 : struct, FooId
            where T7 : struct, FooId
            where T8 : struct, FooId
            where T9 : struct, FooId
            where T10 : struct, FooId
            where T11 : struct, FooId
            where T12 : struct, FooId
            where T13 : struct, FooId
        {
            return new DependentFooId<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(fooId1, fooId2, fooId3, fooId4, fooId5, fooId6, fooId7, fooId8, fooId9, fooId10, fooId11, fooId12, fooId13);
        }

        public FooId<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Upon<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(FooId<T1> fooId1, FooId<T2> fooId2, FooId<T3> fooId3, FooId<T4> fooId4, FooId<T5> fooId5, FooId<T6> fooId6, FooId<T7> fooId7, FooId<T8> fooId8, FooId<T9> fooId9, FooId<T10> fooId10, FooId<T11> fooId11, FooId<T12> fooId12, FooId<T13> fooId13, FooId<T14> fooId14)
            where T1 : struct, FooId
            where T2 : struct, FooId
            where T3 : struct, FooId
            where T4 : struct, FooId
            where T5 : struct, FooId
            where T6 : struct, FooId
            where T7 : struct, FooId
            where T8 : struct, FooId
            where T9 : struct, FooId
            where T10 : struct, FooId
            where T11 : struct, FooId
            where T12 : struct, FooId
            where T13 : struct, FooId
            where T14 : struct, FooId
        {
            return new DependentFooId<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(fooId1, fooId2, fooId3, fooId4, fooId5, fooId6, fooId7, fooId8, fooId9, fooId10, fooId11, fooId12, fooId13, fooId14);
        }

        public FooId<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Upon<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(FooId<T1> fooId1, FooId<T2> fooId2, FooId<T3> fooId3, FooId<T4> fooId4, FooId<T5> fooId5, FooId<T6> fooId6, FooId<T7> fooId7, FooId<T8> fooId8, FooId<T9> fooId9, FooId<T10> fooId10, FooId<T11> fooId11, FooId<T12> fooId12, FooId<T13> fooId13, FooId<T14> fooId14, FooId<T15> fooId15)
            where T1 : struct, FooId
            where T2 : struct, FooId
            where T3 : struct, FooId
            where T4 : struct, FooId
            where T5 : struct, FooId
            where T6 : struct, FooId
            where T7 : struct, FooId
            where T8 : struct, FooId
            where T9 : struct, FooId
            where T10 : struct, FooId
            where T11 : struct, FooId
            where T12 : struct, FooId
            where T13 : struct, FooId
            where T14 : struct, FooId
            where T15 : struct, FooId
        {
            return new DependentFooId<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(fooId1, fooId2, fooId3, fooId4, fooId5, fooId6, fooId7, fooId8, fooId9, fooId10, fooId11, fooId12, fooId13, fooId14, fooId15);
        }

        public FooId<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Upon<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(FooId<T1> fooId1, FooId<T2> fooId2, FooId<T3> fooId3, FooId<T4> fooId4, FooId<T5> fooId5, FooId<T6> fooId6, FooId<T7> fooId7, FooId<T8> fooId8, FooId<T9> fooId9, FooId<T10> fooId10, FooId<T11> fooId11, FooId<T12> fooId12, FooId<T13> fooId13, FooId<T14> fooId14, FooId<T15> fooId15, FooId<T16> fooId16)
            where T1 : struct, FooId
            where T2 : struct, FooId
            where T3 : struct, FooId
            where T4 : struct, FooId
            where T5 : struct, FooId
            where T6 : struct, FooId
            where T7 : struct, FooId
            where T8 : struct, FooId
            where T9 : struct, FooId
            where T10 : struct, FooId
            where T11 : struct, FooId
            where T12 : struct, FooId
            where T13 : struct, FooId
            where T14 : struct, FooId
            where T15 : struct, FooId
            where T16 : struct, FooId
        {
            return new DependentFooId<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(fooId1, fooId2, fooId3, fooId4, fooId5, fooId6, fooId7, fooId8, fooId9, fooId10, fooId11, fooId12, fooId13, fooId14, fooId15, fooId16);
        }

    }
}