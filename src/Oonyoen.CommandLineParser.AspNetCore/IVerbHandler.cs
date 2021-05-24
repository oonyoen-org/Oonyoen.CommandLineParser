namespace Oonyoen.CommandLineParser.AspNetCore
{
    public interface IVerbHandler<TVerb>
    {
        void Handle(TVerb result);
    }
}
