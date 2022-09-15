using System.ComponentModel.DataAnnotations;

namespace SmartIK.Enums
{
    public enum BankEnum
    {
        [Display(Name ="Ziraat Bank")]
        ZiraatBank,
        [Display(Name = "Garanti BBVA Bank")]
        GarantiBBVABank,
        [Display(Name = "TEB Bank")]
        TEBBank,
        [Display(Name = "Kuvety Turk Bank")]
        KuveytTurkBank,
        [Display(Name = "Halk Bank")]
        HalkBank,
        [Display(Name = "Yapı Kredi Bank")]
        YapiKrediBank,
        [Display(Name = "Ak Bank")]
        AkBank,
        [Display(Name = "Is Bank Bank")]
        IsBank
    }
}
