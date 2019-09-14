namespace SnackMachineDDD.logic.Common
{
    public abstract class ValueObject<T>
    where T: ValueObject<T>
    {

        //check for null done (2) and not in each derived class
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
        
        //(1) delegate to derived class and we won't forget to implement it
        protected abstract bool EqualsCore(T other);

        public override int GetHashCode()
        {
            return GetHashCodeCore();
        }

        protected abstract int GetHashCodeCore();
        
        //this is the same as for the entity
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
