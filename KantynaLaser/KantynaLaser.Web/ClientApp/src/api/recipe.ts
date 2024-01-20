export interface Ingredient {
    name: string;
    quantity: number;
    unit: string;
}

export interface Tool {
    name: string;
    amount: number;
}

export interface Step {
    name: string;
}

export interface RecipeDto {
    id: string;
    title: string;
    ingredients: Ingredient[];
    tools: Tool[];
    steps: Step[];
    preparationTime: string;
    cookingTime: string; 
    isPublic: boolean;
    createdAt: string;
}