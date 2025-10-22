const parsePseudoQuery = (query) => {
    const params = {};
    const lines = query.split(/\n|;/).map(l => l.trim()).filter(Boolean);

    lines.forEach(line => {
        const match = line.match(/^(\w+):\s*(.*)$/);
        if (!match) return;

        const key = match[1];
        let value = match[2].trim();

        if (value.startsWith('"') && value.endsWith('"')) {
            value = value.slice(1, -1);
        }

        switch (key) {
            case 'from': params.startDate = value; break;
            case 'to': params.endDate = value; break;
            case 'device': params.device = value; break;
            case 'category': params.category = value; break;
            case 'isLan': params.isLan = value; break;
        }
    });

    return params;
};

export default parsePseudoQuery