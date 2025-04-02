namespace DDPApi.Models
{
    public class ProductToSeans
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int count { get; set; }
        public string barcode { get; set; }
        public int machineId { get; set; }
        public int BatchSize { get; set; }
        public bool isCompleted { get; set; } = false;

        public int status { get; set; } =
            1; // 1 => makineye giriş yaptı | 2 => makineden çıkış yaptı 
    }
}