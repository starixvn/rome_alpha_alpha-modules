namespace Sunplus.Agent.Model.Casino
{
    using CasinoLibrary.SBAgent;

    public interface CasinoService
    {
        Customer Customer { get; }

        Report Report { get; }

        string CasinoLang(string original);
    }
}