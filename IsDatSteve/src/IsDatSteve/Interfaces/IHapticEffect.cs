using System;
namespace IsDatSteve.Interfaces
{
    public interface IHapticEffect
    {
        void HapticSuccess();
        void HapticError();
        void HapticWarning();

        void HapticThud();
        void HapticSelection();
    }
}
