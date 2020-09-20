using KeyChecker.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace KeyChecker.Infrastructure.TestImplementation
{
    internal static class TestApplicationCollection
    {
        public static KeyApplication[] KeyApplications =
        {
            new KeyApplication("Tort", Guid.Parse("854daac1-028e-4f99-8539-ae909a06025f")),
            new KeyApplication("Nep", Guid.Parse("0b6bb660-447b-4cbd-9d8b-c74c37c568d7")),
            new KeyApplication("Kar", Guid.Parse("a61f932d-11d6-4e6a-8f7a-3b9c0593a32d")),
        };
    }
}
