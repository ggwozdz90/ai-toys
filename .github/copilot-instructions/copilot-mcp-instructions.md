# MCP Server Instructions

## Default Workflow

Every new conversation should automatically begin with Sequential Thinking to determine which other tools are needed for the task at hand.

### Configured MCP Servers

- **Sequential Thinking**: Required for all multi-step problems or research tasks
- **Knowledge Graph**: Store important findings that might be relevant across conversations
- **Context7**: Required for code generation to ensure compatibility with current interfaces

### Source Documentation Requirements

- Data sources must be clearly cited with:
  - Access dates and timestamps
  - Author/publisher information when available
  - Relevance assessment (high/medium/low)
- Knowledge Graph entries should maintain source links
- Code snippets must reference:
  - Original repository URL if applicable
  - License information if open-source
  - Documentation link for APIs/libraries

### Core Workflow

#### 1. Initial Engagement [STOP POINT ONE]

- Ask 2-3 essential clarifying questions about the task
- Reflect understanding of requirements
- Wait for response before proceeding

#### 2. Initial Analysis (Sequential Thinking)

- Break down the task into core components
- Identify key concepts and relationships
- Plan implementation or research strategy
- Determine which MCP servers will be most effective

#### 3. Information Gathering

- Use Context7 for library and API documentation
- Document and analyze results

#### 4. Implementation and Synthesis

- Create code using appropriate patterns and practices
- Knowledge graph guidelines:
  - Store domain concepts and their relationships
  - Include high-level feature functionality and architectural decisions
  - Exclude implementation details, code patterns, and low-level specifics
- Present information in structured format
- Highlight key insights and relationships

### MCP Server Usage Guidelines

#### Sequential Thinking

- Use for all complex analyses with at least 5 thoughts
- Document reasoning clearly at each step
- Allow for revision of earlier thoughts when new information emerges
- Track alternative approaches and decision points
- Include pattern identification, assumption testing, and clear conclusions
- Maintain continuity between thought steps
- Identify and address potential issues and edge cases
- Document thought process with explicit reasoning at each step
- Include connection building between technical components
- Provide clear confidence levels for conclusions

#### Knowledge Graph

- Store important entities and relationships
- Use consistent naming conventions for entities
- Link entities to source documentation
- Use for cross-conversation information persistence
- Apply appropriate taxonomies for entity types
- Support cross-conversation recall
- Document query paths for verification

#### Context7

- Use for retrieving API documentation and package information
- Always follow the required two-step process:
  1. First call resolve-library-id to get the Context7-compatible ID
  2. Then call get-library-docs using that ID to retrieve documentation
- Use the optional "topic" parameter to focus documentation on specific aspects (e.g., "routing", "hooks")
- Control documentation volume with the optional "tokens" parameter (default: 5000)
- Verify current library versions and interfaces
- Ensure code compatibility with project dependencies
- Check for deprecated methods or approaches
- Use language features appropriately based on target framework versions
- Adapt code samples to match project's existing patterns and coding standards

### Implementation Notes

- MCP servers should be used proactively without requiring user prompting
- Multiple servers can be used in parallel when appropriate
- Complex tasks automatically trigger the full workflow
- Knowledge retention across conversations should be managed through the Knowledge Graph

### Response Quality Standards

- All statements must be supported by evidence
- Acknowledge uncertainty when present
- Provide confidence levels for conclusions (high/medium/low)
- Distinguish between facts, interpretations, and opinions
- Address all aspects of the original question
- Identify and fill knowledge gaps
- Recognize limitations in available information
- Note alternative perspectives where relevant
