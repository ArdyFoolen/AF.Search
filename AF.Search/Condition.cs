using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF.Search
{
    //public class Condition<T>
    //{
    //    private T value;
    //    private bool condition;

    //    public Condition(T value)
    //    {
    //        this.value = value;
    //    }
    //    public static Condition<T> If(T value, bool condition)
    //        => new Condition<T>(value).When(() => condition);
    //    public static Condition<T> If(T value, Func<bool> predicate)
    //        => new Condition<T>(value).When(predicate);
    //    public Condition<T> When(Func<bool> predicate)
    //    {
    //        condition = predicate();
    //        return this;
    //    }

    //    public Condition<T> Then(Action<T> action)
    //    {
    //        if (condition)
    //            action(value);
    //        return this;
    //    }

    //    private Condition<T> Else(Action<T> action)
    //    {
    //        condition = !condition;
    //        if (condition)
    //            action(value);
    //        return this;
    //    }
    //}
    public class Condition
    {
        private bool condition;

        public static Condition If(bool condition)
            => new Condition().When(() => condition);
        public static Condition If(Func<bool> predicate)
            => new Condition().When(predicate);
 
        public Condition When(Func<bool> predicate)
        {
            condition = predicate();
            return this;
        }

        public Condition Then(Action action)
        {
            if (condition)
                action();
            return this;
        }

        public Condition Else(Action action)
        {
            condition = !condition;
            if (condition)
                action();
            return this;
        }

        public T Return<T>(Func<T> action)
            => action();
    }
}
