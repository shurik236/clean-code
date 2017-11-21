namespace Markdown
{
    public interface IParser
    {
        Token GetNextToken(string str);
    }
}