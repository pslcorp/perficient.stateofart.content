public interface IProcessorData
{
    Task<OutPutResult> ProcessData([NotNull] long id);
}