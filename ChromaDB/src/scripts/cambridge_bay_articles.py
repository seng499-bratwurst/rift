import re

class CambridgeBayArticles:
    """
    Class to handle the cleaning of Cambridge Bay articles.
    """

    def __init__(self, articles):
        """
        Initialize with a list of articles.
        :param articles: List of articles to be cleaned.
        """
        self.articles = articles

    def clean(self):
        """
        Clean the articles by removing unnecessary content.
        :return: List of cleaned articles.
        """
        cleaned_articles = []
        for article in self.articles:
            cleaned_article = self._remove_latex_commands(article)
            cleaned_articles.append(cleaned_article)
        return cleaned_articles

    def create_metadata(self):
        """
        Create metadata for the cleaned articles.
        :return: List of metadata dictionaries for each article.
        """
        metadata = []
        for article in self.articles:
            meta = {
                'source_type': 'article',
                'length': len(article),
                'title': self._extract_title(article),
                'source': self._extract_source(article),
                'id': self._extract_id(article)
            }
            metadata.append(meta)
        return metadata

    @staticmethod
    def _remove_latex_commands(text):
        return re.sub(r'\\[a-zA-Z]+', '', text)

    @staticmethod
    def _extract_source(text):
        match = re.search(r'^# Source:\s*(.+)', text, re.MULTILINE)
        return match.group(1).strip() if match else 'Unknown'

    @staticmethod
    def _extract_id(text):
        match = re.search(r'^# ID:\s*(.+)', text, re.MULTILINE)
        return match.group(1).strip() if match else 'Unknown'

    @staticmethod
    def _extract_title(text):
        source = CAmbridgeBayArticles._extract_source(text)
        title_match = re.search(r'“?(.+?)”?\s*\(Article', source)
        return title_match.group(1).strip() if title_match else source[:80]  # fallback to truncated source