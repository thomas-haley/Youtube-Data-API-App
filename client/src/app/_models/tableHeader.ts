export interface TableHeader
{
    name: string;
    location: string;
    visibleInHeader: boolean;
    customHeaderCSSClass: string;
    customDataCSSClass: string;
    hasHref: boolean;
    href: Function;
}