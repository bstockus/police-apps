using System;
using AutoMapper;

namespace Police.Business.Organization.Officers {

    public class OfficerInfo {

        public Guid Id { get; set; }

        public Guid? UserId { get; set; }
        public string UserUserName { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmailAddress { get; set; }
        public string UserWindowsSid { get; set; }
        public bool? UserIsActive { get; set; }

        public string BadgeNumber { get; set; }
        public string EmployeeNumber { get; set; }
        public string Rank { get; set; }
        public string JobTitle { get; set; }
        public string Assignment { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool? IsCurrentlyEmployed { get; set; }
        public bool? IsSwornOfficer { get; set; }

        public class Mapping : Profile {

            public Mapping() {
                CreateMap<Officer, OfficerInfo>();
            }

        }

    }

}