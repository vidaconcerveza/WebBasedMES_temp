namespace WebBasedMES.Data.Models.Mold
{
    public class MoldLocation
    {
        public int Id { get; set; }
        public string AreaName { get; set; }
        public string Row { get; set; }
        public string Column { get; set; }
        public string Comment { get; set; }
        public int IsUsing { get; set; }


    }
}
