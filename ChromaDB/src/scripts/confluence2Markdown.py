import requests
import trafilatura
from markdownify import markdownify as md
from urllib.parse import urlparse
import os
from tqdm import tqdm
import re

### TO DO:
# Add remaining important information links from ONC Wiki.

OUTPUT_DIR = "Dataset/Markdown/confluence_wiki"
os.makedirs(OUTPUT_DIR, exist_ok=True)

def safe_filename(url):
    parsed = urlparse(url)
    name = parsed.path.strip("/").replace("/", "_") or "index"
    return name.split(".")[0] + ".md"

def clean_table_markdown(markdown):
    """Clean and format table markdown."""
    lines = markdown.split('\n')
    cleaned_lines = []
    in_table = False
    table_lines = []
    
    for line in lines:
        if line.strip().startswith('|'):
            if not in_table:
                in_table = True
                # Add table header if not present
                if not any('---' in l for l in table_lines):
                    headers = [h.strip() for h in line.strip('|').split('|')]
                    table_lines.append('| ' + ' | '.join(headers) + ' |')
                    table_lines.append('|' + '|'.join(['---' for _ in headers]) + '|')
            table_lines.append(line)
        else:
            if in_table:
                # Process collected table lines
                if table_lines:
                    # Ensure proper spacing in table cells
                    cleaned_table = []
                    for table_line in table_lines:
                        cells = [cell.strip() for cell in table_line.strip('|').split('|')]
                        cleaned_table.append('| ' + ' | '.join(cells) + ' |')
                    cleaned_lines.extend(cleaned_table)
                    cleaned_lines.append('')  # Add blank line after table
                table_lines = []
                in_table = False
            cleaned_lines.append(line)
    
    # Handle any remaining table lines
    if table_lines:
        cleaned_table = []
        for table_line in table_lines:
            cells = [cell.strip() for cell in table_line.strip('|').split('|')]
            cleaned_table.append('| ' + ' | '.join(cells) + ' |')
        cleaned_lines.extend(cleaned_table)
    
    return '\n'.join(cleaned_lines)

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
        
        # Clean and format tables
        markdown = clean_table_markdown(markdown)
        
        # Remove any HTML comments
        markdown = re.sub(r'<!--.*?-->', '', markdown, flags=re.DOTALL)
        
        # Remove multiple blank lines
        markdown = re.sub(r'\n{3,}', '\n\n', markdown)

        # Save as .md
        filename = safe_filename(url)
        filepath = os.path.join(OUTPUT_DIR, filename)

        with open(filepath, "w", encoding="utf-8") as f:
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
        "https://wiki.oceannetworks.ca/spaces/O2A/pages/49447553/Available+Locations",
        "https://wiki.oceannetworks.ca/spaces/O2A/pages/48697045/Available+Devices",
        "https://wiki.oceannetworks.ca/spaces/O2A/pages/49449087/Available+Deployments",
        "https://wiki.oceannetworks.ca/spaces/O2A/pages/48697037/Available+Device+Categories",
        "https://wiki.oceannetworks.ca/spaces/O2A/pages/48697051/Available+Properties",
        "https://wiki.oceannetworks.ca/spaces/O2A/pages/48697035/Available+Data+Products"
    ]

    files = batch_process(urls)
    print(f"\n✅ Saved {len(files)} markdown files to `{OUTPUT_DIR}/`")
