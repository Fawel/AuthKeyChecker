using System;
using System.Collections.Generic;
using System.Text;

namespace KeyChecker.Domain
{
    public abstract class AuthKey
    {
        public static NoAuthKey NoKey = new NoAuthKey();
    }
    public class FoundAuthKey : AuthKey
    {
        public readonly AuthKeyValue KeyValue;
        public readonly bool Enabled;
        public readonly KeyApplication Application;
        public readonly KeyApplication AuthAccessedApplication;

        public FoundAuthKey(
            AuthKeyValue keyValue,
            bool enabled,
            KeyApplication application,
            KeyApplication authAccessedApplication)
        {
            KeyValue = keyValue;
            Enabled = enabled;
            Application = application ?? throw new ArgumentNullException(nameof(application));
            AuthAccessedApplication = authAccessedApplication ?? throw new ArgumentNullException(nameof(authAccessedApplication));
        }
    }

    public class NoAuthKey : AuthKey
    {
    }
}
