namespace GraphicClient;

public interface IFileSystemItem
{
    string Name { get; }
    string FullPath { get; }
}