import { mkdir, rm, writeFile } from "node:fs/promises";
import { resolve } from "node:path";
import sharp from "sharp";
import pngToIco from "png-to-ico";

const root = resolve(process.cwd());
const source = resolve(root, "icon.svg");
const output = resolve(root, "src", "app.ico");
const tempDir = resolve(root, ".icon-build");
const sizes = [16, 24, 32, 48, 64, 128, 256];

await rm(tempDir, { recursive: true, force: true });
await mkdir(tempDir, { recursive: true });

const pngPaths = [];

for (const size of sizes) {
  const pngPath = resolve(tempDir, `icon-${size}.png`);
  await sharp(source, { density: 1024 })
    .resize(size, size, {
      fit: "contain",
      background: { r: 0, g: 0, b: 0, alpha: 0 }
    })
    .png()
    .toFile(pngPath);
  pngPaths.push(pngPath);
}

const iconBuffer = await pngToIco(pngPaths);
await writeFile(output, iconBuffer);
await rm(tempDir, { recursive: true, force: true });
