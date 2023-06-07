using System.ComponentModel.DataAnnotations;

namespace Police.Business.ResistanceResponse.Incidents.Reports.Resistances {

    public enum ResistanceType {

        Flee = 1,

        [Display(Name = "Attack Other Person")]
        AttackOtherPerson = 2,
        [Display(Name = "Edged Weapon")] EdgedWeapon = 3,
        [Display(Name = "Threat/Posture")] ThreatOrPosture = 4,
        [Display(Name = "Punch/Hit")] PunchOrHit = 5,
        Firearm = 6,
        [Display(Name = "Pull Away")] PullAway = 7,
        [Display(Name = "Kick/Knee")] KickOrKnee = 8,
        [Display(Name = "Other Weapon")] OtherWeapon = 9,
        [Display(Name = "Other Resistance")] OtherResistance = 10

    }

}