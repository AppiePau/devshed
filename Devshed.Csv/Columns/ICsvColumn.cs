namespace Devshed.Csv
{
    public interface ICsvColumn<TSource>
    {
        string PropertyName { get; }

        string[] Render(CsvDefinition<TSource> defintion, TSource element);

        string[] GetHeaderNames();
    }
}