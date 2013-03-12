namespace SDammann.WebApi.Versioning.Util {
    using System;

    internal struct LockValue<TValue> {
        private bool _isLocked;
        private TValue _value;

        public LockValue(TValue initialValue) {
            this._value = initialValue;
            this._isLocked = false;
        }

        public TValue Value {
            get { return this._value; }
            set {
                if (this._isLocked) {
                    throw new InvalidOperationException("Property cannot be set: already locked");
                }

                this._value = value;
            }
        }

        public bool IsLocked {
            get { return this._isLocked; }
        }

        public void Lock() {
            this._isLocked = true;
        }

        public static implicit operator TValue(LockValue<TValue> obj) {
            return obj.Value;
        }
        public static implicit operator LockValue<TValue>(TValue val) {
            return new LockValue<TValue>(val);
        }
    }
}