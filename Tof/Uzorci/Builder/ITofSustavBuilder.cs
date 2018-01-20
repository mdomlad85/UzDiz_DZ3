﻿namespace Tof.Uzorci.Builder
{
    /// <summary>
    /// The 'Builder' interface
    /// </summary>
    interface ITofSustavBuilder
    {
        void UcitajPostavke(object ulazniPodaci);
        void UcitajPodatke();
        void InicijalizirajSustav();
        void Opremi();
        TofSustav TofSUstav { get; }
    }
}
