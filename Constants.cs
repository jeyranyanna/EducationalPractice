using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HSEPractice2
{
    public static class Constants
    {
        public static class Roles
        {
            public const string Administrator = "Administrator";
            public const string Director = "Генеральный директор";
            public const string Cashier = "Кассир";
            public const string Accountant = "Бухгалтер";
            public const string Instructor = "Инструктор-спасатель";
            public const string Animator = "Аниматор";
            public const string Client = "Клиент";
        }

        public static class Policies
        {
            public const string RequireAdmin = "RequireAdmin";
            public const string RequireDirector = "RequireDirector";
            public const string RequireCashier = "RequireCashier";
            public const string RequireAccountant = "RequireAccountant";
            public const string RequireInstructor = "RequireInstructor";
            public const string RequireAnimator = "RequireAnimator";
            public const string RequireClient = "RequireClient";
        }
    }
}
