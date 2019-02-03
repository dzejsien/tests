using System;

namespace CheckSomeCode
{
    class CheckBoxing : IChecker
    {
        public void Check(Action<string> logMessage, object data)
        {
            bool statusToCheck = true;
            var voStatus = new Status(true);

            if (statusToCheck == voStatus)
                logMessage("Status is true and is equal");
        }
    }

    class Status
    {
        public bool IsActive { get; }

        public Status(bool isActive)
        {
            IsActive = isActive;
        }

        public static implicit operator bool(Status status)
        {
            return status.IsActive;
        }
    }
}
