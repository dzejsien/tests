using System;

namespace CheckSomeCode
{
    class CheckBoxing : IChecker
    {
        public void Check(Action<string> logMessage, object data)
        {
            bool statusToCheck = true;
            var voStatus = new Status(true);

            // no boxing here
            if (statusToCheck == voStatus)
                logMessage("Status is true and is equal");

            // on heap we can see that we have 2 Boolean instances
            object testBox = statusToCheck;
            object test2Box = statusToCheck;
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
