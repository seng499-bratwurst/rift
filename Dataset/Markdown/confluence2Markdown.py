import requests
import trafilatura
from markdownify import markdownify as md
from urllib.parse import urlparse
import os
from tqdm import tqdm

### TO DO:
# Add remaining important information links from ONC Wiki.

OUTPUT_DIR = "markdown_docs"
os.makedirs(OUTPUT_DIR, exist_ok=True)

def safe_filename(url):
    parsed = urlparse(url)
    name = parsed.path.strip("/").replace("/", "_") or "index"
    return name.split(".")[0] + ".md"

def process_url(url):
    try:
        response = requests.get(url, timeout=100)
        response.raise_for_status()

        # Extract clean text
        downloaded = trafilatura.extract(response.text)

        if not downloaded:
            print(f"⚠️ No content found in: {url}")
            return None

        # Convert to Markdown
        markdown = md(downloaded)

        # Save as .md
        filename = safe_filename(url)
        filepath = os.path.join(OUTPUT_DIR, filename)

        with open(filepath, "w", encoding="utf-8") as f:
            f.write(f"# Source: {url}\n\n")
            f.write(markdown)

        return filepath

    except Exception as e:
        print(f"❌ Error processing {url}: {e}")
        return None


def batch_process(url_list):
    saved_files = []
    for url in tqdm(url_list, desc="Processing URLs"):
        result = process_url(url)
        if result:
            saved_files.append(result)
    return saved_files


if __name__ == "__main__":
    # ✅ Replace these with your own list of URLs (docs, wiki, papers, etc.)
    urls = [
        "https://wiki.oceannetworks.ca/spaces/instruments/pages/13598784/Home",
        "https://wiki.oceannetworks.ca/spaces/O2A/pages/49447553/Available+Locations",
        "https://wiki.oceannetworks.ca/spaces/O2A/pages/48697045/Available+Devices",
        "https://wiki.oceannetworks.ca/spaces/O2A/pages/49449087/Available+Deployments?src=contextnavpagetreemode",
        "https://wiki.oceannetworks.ca/spaces/O2A/pages/48697037/Available+Device+Categories",
        "https://wiki.oceannetworks.ca/spaces/O2A/pages/48697051/Available+Properties",
        "https://wiki.oceannetworks.ca/spaces/O2A/pages/48697035/Available+Data+Products"
    ]

    files = batch_process(urls)
    print(f"\n✅ Saved {len(files)} markdown files to `{OUTPUT_DIR}/`")
