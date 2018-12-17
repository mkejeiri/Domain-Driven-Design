namespace SnackMachineDDD.logic
{
    public abstract class ValueObject<T>
    where T: ValueObject<T>
    {

        //this will delegate the real work to abstract EqualsCore
        public override bool Equals(object obj)
        {
            var valueObjectOther = obj as T;
            if (ReferenceEquals(valueObjectOther, null))
            {
                return false;
            }

            return EqualsCore(valueObjectOther);
        }

        protected abstract bool EqualsCore(T other);

        public override int GetHashCode()
        {
            return GetHashCodeCore();
        }

        protected abstract int GetHashCodeCore();
        public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            {
                return true;
            }

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            {
                return false;
            }
            return a.Equals(b);
        }


        public static bool operator !=(ValueObject<T> a, ValueObject<T> b)
        {
            return !(a == b);
        }

}
}
