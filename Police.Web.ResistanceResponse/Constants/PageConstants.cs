namespace Police.Web.ResistanceResponse.Constants {

    public static class AreaConstants {

        public static readonly string ResistanceReport = "ResistanceResponse";

    }

    public static class PageConstants {

        public static readonly string Index = "Index";
        public static readonly string Create = "Create";
        public static readonly string AttachOfficer = "AttachOfficer";
        public static readonly string AttachReport = "AttachReport";
        public static readonly string Details = "Details";
        public static readonly string Delete = "Delete";

        public static readonly string List = "List";

        public static class Modals {

            private static readonly string ModalsBase = "Modals";

            public static class Officer {

                private static readonly string OfficerBase = $"{ModalsBase}/Officer";

                public static readonly string UpdateOfficerInjury = $"{OfficerBase}/UpdateOfficerInjury";
                public static readonly string AttachOfficer = $"{OfficerBase}/AttachOfficer";

            }

            public static class Reports {

                private static readonly string ReportBase = $"{ModalsBase}/Report";

                public static readonly string RemoveReport = $"{ReportBase}/RemoveReport";

            }

            public static class Subject {

                private static readonly string SubjectBase = $"{ModalsBase}/Subject";

                public static readonly string AttachAnimalSubject = $"{SubjectBase}/AttachAnimalSubject";
                public static readonly string UpdateAnimalSubject = $"{SubjectBase}/UpdateAnimalSubject";
                
                public static readonly string AttachPersonSubject = $"{SubjectBase}/AttachPersonSubject";
                public static readonly string UpdatePersonSubject = $"{SubjectBase}/UpdatePersonSubject";
                
                public static readonly string RemoveSubject = $"{SubjectBase}/RemoveSubject";

            }

            public static class Incident {

                private static readonly string IncidentBase = $"{ModalsBase}/Incident";

                public static readonly string UpdateIncidentNumber = $"{IncidentBase}/UpdateIncidentNumber";
                public static readonly string UpdateIncidentDateAndTime = $"{IncidentBase}/UpdateIncidentDateAndTime";

            }

        }

    }

}