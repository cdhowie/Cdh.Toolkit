using System;
using System.Collections.Generic;

namespace Cdh.Toolkit.Extensions.Text
{
    public class Folder
    {
        private int width = 80;

        public int Width
        {
            get { return width; }
            set {
                if (value < 1) {
                    throw new ArgumentOutOfRangeException("value", "Must be >= 1");
                }

                width = value;
            }
        }

        public bool Hyphenate { get; private set; }

        public bool PreserveWhitespace { get; private set; }

        public Folder()
        {
        }

        public IEnumerable<string> Fold(string text)
        {
            Check.ArgumentIsNotNull(text, "text");

            return CreateFoldEnumerable(text);
        }

        // TODO: Collapses consecutive newlines into one.
        private IEnumerable<string> CreateFoldEnumerable(string text)
        {
            int position = 0;

            while (position < text.Length) {
                // Eat leading whitespace if configured to.
                if (!PreserveWhitespace) {
                    while (position < text.Length && char.IsWhiteSpace(text[position])) {
                        ++position;
                    }
                }

                if (position >= text.Length) {
                    break;
                }

                // Compute the potential end of this line.
                int lineEnd = Math.Min(position + width, text.Length);

                // Look for a newline between here and there.
                int newlineWidth;
                var newlinePos = FindNewline(text, position, lineEnd, out newlineWidth);

                if (newlinePos.HasValue) {
                    // Since there is a newline we don't have to wrap anything.
                    yield return text.Substring(position, newlinePos.Value - position);

                    position = newlinePos.Value + newlineWidth;
                } else if (lineEnd == text.Length) {
                    // No newline, and the remaining text fits on one line.  We are done.
                    yield return text.Substring(position, lineEnd - position);
                    yield break;
                } else {
                    // No newline.  We need to wrap.  Find the last whitespace character.  We add 1 to lineEnd because
                    // if the next character after the line is whitespace, then this line fits perfectly and we don't
                    // need to break it up at all.
                    var whitePos = FindLastWhitespace(text, position, lineEnd + 1);

                    if (!whitePos.HasValue) {
                        // No whitespace found.  We need to break up a word.
                        if (Hyphenate) {
                            --lineEnd;
                        }

                        var piece = text.Substring(position, lineEnd - position);

                        if (Hyphenate) {
                            piece += "-";
                        }

                        yield return piece;

                        position = lineEnd;
                    } else {
                        // There is whitespace.  If the found whitespace is the current character then
                        // PreserveWhitespace must be true (or we would have eaten the whitespace).  So yield the whole
                        // line since it's all whitespace.
                        if (whitePos.Value == position) {
                            yield return text.Substring(position, lineEnd - position);
                            position = lineEnd;
                        } else {
                            // In the other case, we have a good spot to split.
                            yield return text.Substring(position, whitePos.Value - position);
                            position = whitePos.Value;
                        }
                    }
                }
            }
        }

        private static int? FindLastWhitespace(string text, int begin, int end)
        {
            int? whitePos = null;

            for (int pos = end - 1; pos >= begin; --pos) {
                if (char.IsWhiteSpace(text[pos])) {
                    whitePos = pos;
                } else if (whitePos.HasValue) {
                    // If this is true then we saw whitespace, but the current character is not whitespace.  So we
                    // return the last position we saw, and this will be the first whitespace character in the last
                    // block of whitespace.
                    return whitePos;
                }
            }

            return whitePos;
        }

        private static int? FindNewline(string text, int begin, int end, out int width)
        {
            width = 1;

            for (int pos = begin; pos < end; ++pos) {
                if (text[pos] == '\r') {
                    // CR-style ending.  Check for CRLF.
                    var lfPos = pos + 1;
                    if (lfPos < end && text[lfPos] == '\n') {
                        width = 2;
                    }

                    return pos;
                } else if (text[pos] == '\n') {
                    // LF-style ending.
                    return pos;
                }
            }

            return null;
        }
    }
}

