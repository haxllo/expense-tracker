# Installing Geist Fonts for ExpenseTracker

## Quick Start

Your ExpenseTracker app now uses **Geist Sans** and **Geist Mono** fonts from Vercel for a modern, professional look.

## Option 1: Install Geist Fonts (Recommended)

### Step 1: Download Geist Fonts

1. Visit the official Geist Font releases page:
   **https://github.com/vercel/geist-font/releases/latest**

2. Download the latest release (e.g., `Geist-1.5.1.zip`)

3. Extract the ZIP file

### Step 2: Install on Windows

1. Navigate to the extracted folder
2. Find the `otf` or `ttf` folder
3. Look for these files:
   - `Geist-Regular.otf` (or .ttf)
   - `Geist-Medium.otf`
   - `Geist-SemiBold.otf`
   - `Geist-Bold.otf`
   - `GeistMono-Regular.otf`
   - `GeistMono-Medium.otf`
   - `GeistMono-SemiBold.otf`
   - `GeistMono-Bold.otf`

4. **Select all the Geist font files** you want to install
5. **Right-click** on the selected files
6. Choose **"Install for all users"** (requires admin) or **"Install"** (current user only)

### Step 3: Restart ExpenseTracker

1. Close the ExpenseTracker app if it's running
2. Launch ExpenseTracker again
3. The app will now use Geist fonts!

## Option 2: Use System Fonts (No Installation Required)

If you don't install Geist fonts, **the app will automatically use fallback fonts**:

- **Segoe UI** (primary font fallback on Windows)
- **Consolas** (monospace fallback on Windows)

The app is designed to work perfectly with both Geist and fallback fonts.

## Verifying Installation

### Windows Font Viewer Method:
1. Open **Settings** → **Personalization** → **Fonts**
2. Search for "Geist"
3. You should see "Geist" and "Geist Mono" in the list

### Visual Verification:
1. Launch ExpenseTracker
2. Text should appear slightly more modern and refined with Geist
3. Compare with screenshots in the README (if available)

## Font Characteristics

### Geist Sans
- **Style**: Geometric sans-serif
- **Best For**: UI text, headings, body content
- **Characteristics**: Clean, modern, highly readable

### Geist Mono
- **Style**: Monospaced
- **Best For**: Code, numbers, tabular data
- **Characteristics**: Clear character distinction, coding-friendly

## Troubleshooting

### Fonts not appearing after installation?

1. **Verify installation**:
   - Open Windows Fonts folder: `C:\Windows\Fonts`
   - Search for "Geist"

2. **Restart your computer** (sometimes required for font registration)

3. **Check file permissions**:
   - Make sure you installed as administrator
   - Try "Install for all users" option

### Still using fallback fonts?

This is normal and intentional! The app works great with Segoe UI if Geist isn't installed.

## Resources

- **Official Website**: https://vercel.com/font
- **GitHub Repository**: https://github.com/vercel/geist-font
- **License**: SIL Open Font License 1.1 (Free for personal and commercial use)

## Technical Details

Font configuration is managed in:
- `ExpenseTracker/Themes/ModernTheme.xaml`

Font fallback chain:
```xml
<!-- Primary Font -->
Geist → Segoe UI → Inter → system-ui → Arial

<!-- Monospace Font -->
Geist Mono → Consolas → SF Mono → Monaco → Courier New
```

## Support

If you encounter any issues with font installation or display, please check:
1. Windows Font settings
2. ExpenseTracker restart
3. Font file integrity (re-download if corrupted)
