# Installer

The production installer target is Inno Setup.

Rules:

- Binaries install to the selected app install directory only.
- User data remains in `%AppData%/Hstar Canvas` or `%LocalAppData%/Hstar Canvas`.
- Uninstall preserves business data by default.
- Upgrade closes the running app before overwrite.

