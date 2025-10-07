# Installing JetBrains Mono Font for ExpenseTracker

## Quick Start

Your ExpenseTracker app now uses **JetBrains Mono** font for a modern, developer-friendly look with excellent readability.

## Option 1: Install JetBrains Mono Font (Recommended)

### Step 1: Download JetBrains Mono

1. Visit the official JetBrains Mono page:
   **https://www.jetbrains.com/lp/mono/**

2. Click the **"Download font"** button

3. Or visit GitHub releases:
   **https://github.com/JetBrains/JetBrainsMono/releases/latest**

4. Extract the ZIP file

### Step 2: Install on Windows

1. Navigate to the extracted folder
2. Go to `fonts/ttf` folder
3. Look for these files:
   - `JetBrainsMono-Regular.ttf`
   - `JetBrainsMono-Medium.ttf`
   - `JetBrainsMono-SemiBold.ttf`
   - `JetBrainsMono-Bold.ttf`

4. **Select all the JetBrainsMono font files** you want to install
5. **Right-click** on the selected files
6. Choose **"Install for all users"** (requires admin) or **"Install"** (current user only)

### Step 3: Restart ExpenseTracker

1. Close the ExpenseTracker app if it's running
2. Launch ExpenseTracker again
3. The app will now use Geist fonts!

## Option 2: Use System Fonts (No Installation Required)

If you don't install JetBrains Mono, **the app will automatically use fallback fonts**:

- **Consolas** (excellent monospace fallback on Windows)
- **Segoe UI** (clean sans-serif fallback on Windows)

The app is designed to work perfectly with both JetBrains Mono and fallback fonts.

## Verifying Installation

### Windows Font Viewer Method:
1. Open **Settings** → **Personalization** → **Fonts**
2. Search for "JetBrains"
3. You should see "JetBrains Mono" in the list

### Visual Verification:
1. Launch ExpenseTracker
2. Text should appear monospaced with excellent clarity
3. Numbers should be very distinct and easy to read

## Font Characteristics

### JetBrains Mono
- **Style**: Monospaced typeface
- **Best For**: UI text, numbers, data displays, financial applications
- **Characteristics**: 
  - Increased letter height for better readability
  - Distinct character shapes (no confusion between I, l, 1, or O, 0)
  - Designed specifically for developers
  - Clean, modern appearance
  - Perfect for expense tracking with clear number distinction
  - Excellent readability even at small sizes

## Troubleshooting

### Fonts not appearing after installation?

1. **Verify installation**:
   - Open Windows Fonts folder: `C:\Windows\Fonts`
   - Search for "JetBrains"

2. **Restart your computer** (sometimes required for font registration)

3. **Check file permissions**:
   - Make sure you installed as administrator
   - Try "Install for all users" option

### Still using fallback fonts?

This is normal and intentional! The app works great with Consolas if JetBrains Mono isn't installed.

## Resources

- **Official Website**: https://www.jetbrains.com/lp/mono/
- **GitHub Repository**: https://github.com/JetBrains/JetBrainsMono
- **License**: OFL-1.1 (Free for personal and commercial use)

## Technical Details

Font configuration is managed in:
- `ExpenseTracker/Themes/ModernTheme.xaml`

Font fallback chain:
```xml
<!-- Primary Font -->
JetBrains Mono → Consolas → Segoe UI → system-ui → Arial

<!-- Monospace Font -->
JetBrains Mono → Consolas → SF Mono → Monaco → Courier New
```

## Support

If you encounter any issues with font installation or display, please check:
1. Windows Font settings
2. ExpenseTracker restart
3. Font file integrity (re-download if corrupted)
