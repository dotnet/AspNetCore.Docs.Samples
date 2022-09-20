// Custom Http Request Feature
class MathErrorFeature
{
    public MathErrorType MathError { get; set; }
}

// Custom math errors
enum MathErrorType
{
    DivisionByZeroError,
    NegativeRadicandError
}
