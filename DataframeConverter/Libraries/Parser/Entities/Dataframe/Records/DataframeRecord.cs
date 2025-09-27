using IE.Entities.Dataframe.Abstract;

namespace IE.Entities.Dataframe;

class DataframeRecord
{
    DataframeFieldGroup? _currentGroup = null!;

    public string Name { get; set; } = "";

    public string Level { get; set; } = "";

    public string IdentifiedBy { get; set; } = "";

    public string Key { get; set; } = "";

    public DataframeFieldGroup? RootGroup { get; private set; } = null;

    public void QueueField(DataframeField field)
    {
        if (RootGroup is null)
        {
            RootGroup = (DataframeFieldGroup)field;
            _currentGroup = (DataframeFieldGroup)field;
            return;
        }

        if (field.Depth > _currentGroup!.Depth)
        {
            AddFieldToCurrentGroup(field);
        }
        else
        {
            while (field.Depth <= _currentGroup!.Depth)
            {
                _currentGroup = _currentGroup!.ParentGroup;
            }

            if (_currentGroup is null)
            {
                throw new ArgumentException($"Failed to add field with Depth = '{field.Depth}'");
            }
            else
            {
                AddFieldToCurrentGroup(field);
            }
        }
    }

    void AddFieldToCurrentGroup(DataframeField field)
    {
        _currentGroup!.ChildFields.Add(field);
        field.ParentGroup = _currentGroup;

        if (field is DataframeFieldGroup group)
        {
            _currentGroup = group;
        }
    }
}