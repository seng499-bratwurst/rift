import re

class ResearchPapers:
    def __init__(self, papers):
        self.papers = papers

    def clean(self):
        cleaned_papers = []
        for paper in self.papers:
            cleaned_paper = self._remove_latex_commands(paper)
            cleaned_papers.append(cleaned_paper)
        return cleaned_papers

    def create_metadata(self):
        metadata = []
        for paper in self.papers:
            meta = {
                'source_type': 'paper',
                'length': len(paper),
                'title': self._extract_title(paper),
                'source': self._extract_source(paper),
                'id': self._extract_id(paper)
            }
            metadata.append(meta)
        return metadata

    @staticmethod
    def _remove_latex_commands(text):
        return re.sub(r'\\[a-zA-Z]+', '', text)

    @staticmethod
    def _extract_source(text):
        match = re.search(r'^# Source:\s*(.+)', text, re.MULTILINE)
        return match.group(1).strip() if match and match.group(1) else 'Unknown'

    @staticmethod
    def _extract_id(text):
        match = re.search(r'^# ID:\s*(.+)', text, re.MULTILINE)
        return match.group(1).strip() if match and match.group(1) else 'Unknown'

    @staticmethod
    def _extract_title(text):
        """
        Optionally try to pull the paper title from the source line if no other formatting is used.
        """
        source = ResearchPapers._extract_source(text)
        if not source or source == 'Unknown':
            return 'Untitled'

        title_match = re.search(r'“?(.+?)”?\s*\(Doctoral|Master|Thesis', source)
        return title_match.group(1).strip() if title_match and title_match.group(1) else source[:80]