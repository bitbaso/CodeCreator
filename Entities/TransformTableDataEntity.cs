namespace CodeCreator.Entities;

public class TransformTableDataEntity : TableDataEntity
{
    public string NamespaceName { get; set; }
    public List<TransformColumnDataEntity> TransformColumns { get; set; }

}
