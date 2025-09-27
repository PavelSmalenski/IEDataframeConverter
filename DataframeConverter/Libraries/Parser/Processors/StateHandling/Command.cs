namespace IE.Parsers.Processors.StateHandling;

enum Command
{
    NewPage, // '1'
    Advance1, // ' '
    Advance2, // '0'
    Advance3, // '-'
    DataframeOrganized, // Custom rule
    RecordHeading, // Custom rule
    RecordIdentified, // Custom rule
    RecordKey, // Custom rule
    Exit // Custom rule
}