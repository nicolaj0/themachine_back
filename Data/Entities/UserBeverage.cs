namespace CoreCodeCamp.Data
{
  public class UserBeverage
  {
    public int UserBeverageId { get; set; }
    public int BeverageType { get; set; }
    public bool UseOwnMug { get; set; }
    public int Sugar { get; set; }
    public MachineUser User { get; set; }

  }
}