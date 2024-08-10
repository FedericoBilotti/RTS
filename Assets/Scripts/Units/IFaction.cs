using Player;

namespace Units
{
    public interface IFaction
    {
        EFaction GetFaction();
        void SetFaction(EFaction faction);
    }
}