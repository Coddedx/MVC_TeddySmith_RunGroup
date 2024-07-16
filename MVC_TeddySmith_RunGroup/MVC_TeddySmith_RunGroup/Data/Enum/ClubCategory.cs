namespace MVC_TeddySmith_RunGroup.Data.Enum
{
    //Sometimes we must use the constant variable in our application to not be changed throughout the application.
    //One of the methods to declare the continuous variables are Enum
    //Enums' key benefit is to make it possible in the future to adjust values.
    //they store memory value. 
    public enum ClubCategory  //If you have a set of functionally important and unchanged values
    {
        //hafızada 0,1,2 diye saklanır. city=3 yaparsak trail 4, Endurance 5 diye saklanır
        RoadRunner,  
        Womens,
        City,
        Trail,
        Endurance
    }
}
