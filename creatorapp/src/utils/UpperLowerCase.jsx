// utils/UpperLowerCase.jsx

export function safeUpper(input, context = 'unknown') {
    if (typeof input !== 'string') {
        console.warn(`[safeUpper] Expected string but got:`, {
            value: input,
            context,
        });
        return '';
    }
    return input.toUpperCase();
}

export function safeLower(input, context = 'unknown') {
    if (typeof input !== 'string') {
        console.warn(`[safeLower] Expected string but got:`, {
            value: input,
            context,
        });
        return '';
    }
    return input.toLowerCase();
}
