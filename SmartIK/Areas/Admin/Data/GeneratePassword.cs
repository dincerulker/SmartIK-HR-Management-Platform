using System.Text;

namespace SmartIK.Areas.Admin.Data
{
    public class GeneratePassword
    {

        Random _rnd;
        public GeneratePassword()
        {
            _rnd = new Random();
        }

        public int RastgeleSayi(int min, int max)
        {
            return _rnd.Next(min, max);
        }

        public string RasteleHarf(int boyut, bool kucukHarf)
        {

            string harfler = "";
            int sayi, min = 65;
            char harf;

            if (kucukHarf)
            {
                min = 97;
            }

            for (int i = 0; i < boyut; i++)
            {
                sayi = _rnd.Next(min, min + 25);
                harf = Convert.ToChar(sayi);
                harfler += harf;
            }
            return harfler;
        }
        public string Karakter()
        {
            Random rastgele = new Random();
            char[] karakterler = { '.', ',', '_', '-', '*' };
            int sayi = rastgele.Next(5);
            string karakter = Convert.ToString(karakterler[sayi]);
            return karakter;

        }


        public string sifreUret()
        {

            Random rnd = new Random();
            StringBuilder builder = new StringBuilder();
            builder.Append(RasteleHarf(2, true));
            builder.Append(Karakter());
            builder.Append(rnd.Next(10, 99));
            builder.Append(RasteleHarf(1, false));
            return builder.ToString();
        }

    }

}
