#!/bin/bash
# Script to create a proper macOS .app bundle from published files
# Based on Avalonia macOS deployment documentation

set -e

RUNTIME=$1
APP_NAME="Clac"
BUNDLE_ID="com.clac.app"
EXECUTABLE_NAME="Clac.UI"

if [ -z "$RUNTIME" ]; then
    echo "Usage: $0 <runtime> (e.g., osx-x64)"
    exit 1
fi

PUBLISH_DIR="publish/$RUNTIME"
APP_BUNDLE="${APP_NAME}.app"
CONTENTS_DIR="${APP_BUNDLE}/Contents"
MACOS_DIR="${CONTENTS_DIR}/MacOS"
RESOURCES_DIR="${CONTENTS_DIR}/Resources"

# Verify publish directory exists
if [ ! -d "$PUBLISH_DIR" ]; then
    echo "Error: Publish directory not found: $PUBLISH_DIR"
    exit 1
fi

# Verify executable exists
if [ ! -f "$PUBLISH_DIR/$EXECUTABLE_NAME" ]; then
    echo "Error: Executable not found: $PUBLISH_DIR/$EXECUTABLE_NAME"
    echo "Make sure UseAppHost=true is set in the project file"
    exit 1
fi

# Create .app bundle structure
mkdir -p "$MACOS_DIR"
mkdir -p "$RESOURCES_DIR"

# Move all published files into MacOS directory
cp -R "$PUBLISH_DIR"/* "$MACOS_DIR/"

# Make executable
chmod +x "${MACOS_DIR}/${EXECUTABLE_NAME}"

# Create Info.plist
cat > "${CONTENTS_DIR}/Info.plist" <<EOF
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>CFBundleDevelopmentRegion</key>
    <string>en</string>
    <key>CFBundleExecutable</key>
    <string>${EXECUTABLE_NAME}</string>
    <key>CFBundleIdentifier</key>
    <string>${BUNDLE_ID}</string>
    <key>CFBundleInfoDictionaryVersion</key>
    <string>6.0</string>
    <key>CFBundleName</key>
    <string>${APP_NAME}</string>
    <key>CFBundlePackageType</key>
    <string>APPL</string>
    <key>CFBundleShortVersionString</key>
    <string>1.0</string>
    <key>CFBundleVersion</key>
    <string>1</string>
    <key>LSMinimumSystemVersion</key>
    <string>10.15</string>
    <key>NSHighResolutionCapable</key>
    <true/>
</dict>
</plist>
EOF

echo "Created ${APP_BUNDLE}"
echo "Bundle structure:"
echo "  ${APP_BUNDLE}/Contents/MacOS/${EXECUTABLE_NAME}"
echo "  ${APP_BUNDLE}/Contents/Info.plist"
