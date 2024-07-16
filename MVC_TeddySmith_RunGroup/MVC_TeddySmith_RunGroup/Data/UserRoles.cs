namespace MVC_TeddySmith_RunGroup.Data
{
    public static class UserRoles //static yaptık ki kolayca classlara getirelim.Modelde kullanmayacağımız için models içinde olmasına gerek yok
    {
        public const string Admin = "admin";
        public const string User = "user";
        //Seed e gidip gerekli yerleri yazıyoruz sonrasında program.cs de builder services i kuruyoruz identity framework için 

    }
}
