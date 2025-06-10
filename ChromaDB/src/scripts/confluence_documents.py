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