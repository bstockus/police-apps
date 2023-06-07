using System.ComponentModel.DataAnnotations;

namespace Police.Business.ResistanceResponse.Incidents.Subjects.People {

    public enum SuspectedUse {

        None = 0,
        Alcohol = 1,
        Drugs = 2,
        [Display(Name = "Drugs and Alcohol")] DrugsAndAlcohol = 3,
        Unknown = -1

    }

}