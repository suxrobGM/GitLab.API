
namespace GitLab.API
{
    public enum VersionApi
    {
        v1 = 1,
        v2,
        v3,
        v4
    }

    public enum AccessLevel
    {
        Guest = 10,
        Reporter = 20,
        Developer = 30,
        Maintainer = 40,
        Owner = 50 // Only valid for groups
    }

    public enum ArchiveFormat
    {
        TarGz,
        TarBz2,
        Tbz,
        Tbz2,
        Tb2,
        Bz2,
        Tar,
        Zip
    }
}
