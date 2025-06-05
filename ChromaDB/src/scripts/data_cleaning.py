class ConfluenceDocuments:
    """
    Class to handle the cleaning of Confluence documents.
    """

    def __init__(self, docs):
        """
        Initialize with a list of documents.
        :param docs: List of documents to be cleaned.
        """
        self.docs = docs

    def clean(self):
        """
        Clean the documents by removing unnecessary content.
        :return: List of cleaned documents.
        """
        cleaned_docs = []
        for doc in self.docs:
            # Example cleaning operation: remove HTML tags
            cleaned_doc = self._remove_html_tags(doc)
            cleaned_docs.append(cleaned_doc)
        return cleaned_docs
    
    def create_metadata(self):
        """
        Create metadata for the cleaned documents.
        :return: List of metadata dictionaries for each document.
        """
        metadata = []
        for doc in self.docs:
            # Example metadata creation: just the length of the document
            metadata.append({'length': len(doc)})
        return metadata

    @staticmethod
    def _remove_html_tags(text):
        """
        Remove HTML tags from the text.
        :param text: Text from which to remove HTML tags.
        :return: Text without HTML tags.
        """
        import re
        clean = re.compile('<.*?>')
        return re.sub(clean, '', text)
    
class ResearchPapers:
    """
    Class to handle the cleaning of research papers.
    """

    def __init__(self, papers):
        """
        Initialize with a list of research papers.
        :param papers: List of research papers to be cleaned.
        """
        self.papers = papers

    def clean(self):
        """
        Clean the research papers by removing unnecessary content.
        :return: List of cleaned research papers.
        """
        cleaned_papers = []
        for paper in self.papers:
            # Example cleaning operation: remove LaTeX commands
            cleaned_paper = self._remove_latex_commands(paper)
            cleaned_papers.append(cleaned_paper)
        return cleaned_papers

    @staticmethod
    def _remove_latex_commands(text):
        """
        Remove LaTeX commands from the text.
        :param text: Text from which to remove LaTeX commands.
        :return: Text without LaTeX commands.
        """
        import re
        return re.sub(r'\\[a-zA-Z]+', '', text)  # Simple regex to remove LaTeX commands
    
    def create_metadata(self):
        """
        Create metadata for the cleaned research papers.
        :return: List of metadata dictionaries for each paper.
        """
        metadata = []
        for paper in self.papers:
            # Example metadata creation: just the length of the paper
            metadata.append({'length': len(paper)})
        return metadata