namespace Units.Resources
{
    public class Wood : Resource
    {
        public override int ProvideResource()
        {
            int amount = resource.AmountToGive;
            actualAmountOfResource -= amount;

            if (actualAmountOfResource <= 0)
            {
                gameObject.SetActive(false);
            }

            return amount;
        }
    }
}